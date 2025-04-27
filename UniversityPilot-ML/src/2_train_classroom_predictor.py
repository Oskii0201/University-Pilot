import pandas as pd
import numpy as np
from sklearn.model_selection import train_test_split
from sklearn.preprocessing import OneHotEncoder, LabelEncoder
from xgboost import XGBClassifier
import joblib

# Parametr: ile sal chcesz w predykcji?
TOP_N = 5

# 1. Wczytanie danych
df = pd.read_csv('../HistoricalData/planzajec.csv')
sal_freq = pd.read_csv('../HistoricalData/przedmioty_z_salami.csv')
classroom_df = pd.read_csv('../DataInput/Classrooms.csv')

# 2. Filtruj tylko używane sale
saly_kluczowe = classroom_df['Number'].unique()
df = df[df['sala'].isin(saly_kluczowe)]

# 3. Pomijamy wykłady i online
df = df[~df['rodz. zajec'].isin(['wykład', 'zajęcia online'])]

# 4. Usuń rzadkie klasy sal
min_samples = 10
sala_counts = df['sala'].value_counts()
czeste_sale = sala_counts[sala_counts >= min_samples].index
df = df[df['sala'].isin(czeste_sale)]

# 5. Oblicz frequency_score
sal_freq['total_for_subject'] = sal_freq.groupby('przedmiot')['LiczbaZajec'].transform('sum')
sal_freq['frequency_score'] = sal_freq['LiczbaZajec'] / sal_freq['total_for_subject']
sal_freq = sal_freq.rename(columns={'Przedmiot': 'przedmiot', 'Sala': 'sala'})
df = pd.merge(df, sal_freq[['przedmiot', 'sala', 'frequency_score']], on=['przedmiot', 'sala'], how='left')
df['frequency_score'] = df['frequency_score'].fillna(0)

# 6. One-hot encoding dla wejścia
ohe_przedmiot = OneHotEncoder()
ohe_rodzaj = OneHotEncoder()
ohe_grupa = OneHotEncoder()

przedmiot_enc = ohe_przedmiot.fit_transform(df[['przedmiot']]).toarray()
rodzaj_enc = ohe_rodzaj.fit_transform(df[['rodz. zajec']]).toarray()
grupa_enc = ohe_grupa.fit_transform(df[['grupa']]).toarray()
freq_score_enc = df[['frequency_score']].values  # nowa cecha!

# 7. Finalne dane wejściowe
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
print(f"\n🎯 Top-1 Accuracy: {results['Top1_Correct'].mean():.4f}")
print(f"📊 Top-{TOP_N} zawiera poprawną salę w: {(results.apply(lambda row: row['Sala_Prawdziwa'] in row[[f'Top{i+1}' for i in range(TOP_N)]].values, axis=1)).mean():.4f}")

# 14. Zapis modelu i enkoderów
#model.save_model('xgb_topN_sala_model.json')
#joblib.dump(ohe_przedmiot, 'onehot_encoder_przedmiot.pkl')
#joblib.dump(ohe_rodzaj, 'onehot_encoder_rodzaj.pkl')
#joblib.dump(ohe_grupa, 'onehot_encoder_grupa.pkl')
#joblib.dump(le_sala, 'label_encoder_sala.pkl')
results.to_csv('../DataOutput/TopN_Classroom_Predictions.csv', index=False)

# ... cały Twój kod do punktu 13 włącznie (czyli do zapisania resultów)
'''
# 15. Porównanie ML z historycznymi danymi
import ast

# 15.1 Wczytanie i przygotowanie danych historycznych
freq_df = pd.read_csv('przedmioty_z_salami.csv')  # Ten sam plik co wcześniej
freq_df['sala'] = freq_df['sala'].apply(lambda x: ast.literal_eval(x) if isinstance(x, str) else [])
exploded_df = freq_df.explode('sala')

sala_freq = (
    exploded_df
    .groupby(['przedmiot', 'rodz. zajec', 'sala'])
    .size()
    .reset_index(name='frequency')
    .sort_values(['przedmiot', 'rodz. zajec', 'frequency'], ascending=[True, True, False])
)

# 15.2 Funkcja pomocnicza
def get_top_n_sale(przedmiot, rodzaj, n=TOP_N):
    result = sala_freq[
        (sala_freq['przedmiot'] == przedmiot) &
        (sala_freq['rodz. zajec'] == rodzaj)
    ].head(n)
    return result['sala'].tolist()

# 15.3 Sprawdzenie zgodności modelu ML z historią
results['TopN_history'] = results.apply(
    lambda row: get_top_n_sale(row['Przedmiot'], row['Rodzaj'], n=TOP_N),
    axis=1
)
results['Top1_in_history'] = results.apply(
    lambda row: row['Top1'] in row['TopN_history'],
    axis=1
)

print(f"\n📚 Top1 z modelu pokrywa się z historycznymi top-{TOP_N}: {results['Top1_in_history'].mean():.4f}")
'''