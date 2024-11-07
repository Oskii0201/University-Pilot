import axios from "axios";

const apiClient = axios.create({
  baseURL: "https://localhost:7227/api",
  withCredentials: true,
});
export default apiClient;
