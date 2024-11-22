import apiClient from "./apiClient";

export const login = async (email: string, password: string) => {
  try {
    const response = await apiClient.post("/account/login", {
      email,
      password,
    });
    return response.data;
  } catch (e) {
    const errorMessage =
      e instanceof Error ? e.message : "Wystąpił nieznany błąd.";
    throw new Error(
      `Błąd logowania. Sprawdź swoje dane. Szczegóły: ${errorMessage}`,
    );
  }
};

export const logout = async () => {
  try {
    const response = await apiClient.post("/account/logout");
    window.location.href = "/";
    return response.data;
  } catch (error) {
    console.error("Błąd wylogowania:", error);
  }
};
