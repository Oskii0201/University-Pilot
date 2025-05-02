import pandas as pd
import numpy as np
from sklearn.model_selection import train_test_split
from sklearn.preprocessing import OneHotEncoder, LabelEncoder
from xgboost import XGBClassifier
import joblib


TOP_N = 5


df = pd.read_csv('../HistoricalData/planzajec.csv')
sal_freq = pd.read_csv('../HistoricalData/przedmioty_z_salami.csv')
classroom_df = pd.read_csv('../DataInput/Classrooms.csv')

saly_kluczowe = classroom_df['Number'].unique()
df = df[df['sala'].isin(saly_kluczowe)]

df = df[~df['rodz. zajec'].isin(['wykład', 'zajęcia online'])]

min_samples = 10
sala_counts = df['sala'].value_counts()
czeste_sale = sala_counts[sala_counts >= min_samples].index
df = df[df['sala'].isin(czeste_sale)]

sal_freq['total_for_subject'] = sal_freq.groupby('przedmiot')['LiczbaZajec'].transform('sum')
sal_freq['frequency_score'] = sal_freq['LiczbaZajec'] / sal_freq['total_for_subject']
sal_freq = sal_freq.rename(columns={'Przedmiot': 'przedmiot', 'Sala': 'sala'})

df = pd.merge(df, sal_freq[['przedmiot', 'sala', 'frequency_score']], on=['przedmiot', 'sala'], how='left')
df['frequency_score'] = df['frequency_score'].fillna(0)

ohe_przedmiot = OneHotEncoder()
ohe_rodzaj = OneHotEncoder()
ohe_grupa = OneHotEncoder()

przedmiot_enc = ohe_przedmiot.fit_transform(df[['przedmiot']]).toarray()
rodzaj_enc = ohe_rodzaj.fit_transform(df[['rodz. zajec']]).toarray()
grupa_enc = ohe_grupa.fit_transform(df[['grupa']]).toarray()
freq_score_enc = df[['frequency_score']].values  # nowa cecha!

X = np.concatenate([przedmiot_enc, rodzaj_enc, grupa_enc, freq_score_enc], axis=1)

# 8. Label encoding sali
le_sala = LabelEncoder()
y = le_sala.fit_transform(df['sala'])

# 9. Zachowanie indeksów przed podziałem
indices = df.index
X_train, X_test, y_train, y_test, train_indices, test_indices = train_test_split(
    X, y, indices, test_size=0.2, random_state=42)

# 10. Trening modelu
model = XGBClassifier(
    objective='multi:softprob',
    num_class=len(np.unique(y)),
    eval_metric='mlogloss',
    n_estimators=200,
    learning_rate=0.3,
    max_depth=8
)
model.fit(X_train, y_train)

# 11. Predykcja top-N sal
proba = model.predict_proba(X_test)
topN_preds = np.argsort(proba, axis=1)[:, -TOP_N:][:, ::-1]
topN_labels = np.vectorize(lambda idx: le_sala.inverse_transform([idx])[0])(topN_preds)

# 12. Tworzenie wyników
results = pd.DataFrame({
    'Przedmiot': df.loc[test_indices, 'przedmiot'].values,
    'Rodzaj': df.loc[test_indices, 'rodz. zajec'].values,
    'Sala_Prawdziwa': le_sala.inverse_transform(y_test),
})
for i in range(TOP_N):
    results[f'Top{i+1}'] = topN_labels[:, i]
results['Top1_Correct'] = results['Sala_Prawdziwa'] == results['Top1']

# 13. Accuracy i zapis
print(results.head(10))
print(f"\n Top-1 Accuracy: {results['Top1_Correct'].mean():.4f}")
print(f"Top-{TOP_N} zawiera poprawną salę w: {(results.apply(lambda row: row['Sala_Prawdziwa'] in row[[f'Top{i+1}' for i in range(TOP_N)]].values, axis=1)).mean():.4f}")

results.to_csv('../DataOutput/TopN_Classroom_Predictions.csv', index=False)

