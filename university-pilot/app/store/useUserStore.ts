import { create } from "zustand";
import { User } from "@/app/types";
import apiClient from "@/app/lib/apiClient";

interface UserStore {
  user: User | null;
  fetchUser: () => Promise<void>;
}

export const useUserStore = create<UserStore>((set) => ({
  user: null,

  fetchUser: async () => {
    try {
      const response = await apiClient.get("/Account/LoggedUserDetails");

      set({ user: response.data });
    } catch (error) {
      console.error("Błąd pobierania użytkownika:", error);
      set({ user: null });
    }
  },
}));
