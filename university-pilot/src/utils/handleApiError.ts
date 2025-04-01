import { AxiosError } from "axios";

export function handleApiError(error: unknown): string {
  if (error instanceof AxiosError) {
    return error.response?.data?.error || "Failed to fetch data from API";
  }
  if (error instanceof Error) {
    return error.message;
  }
  if (typeof error === "object" && error !== null && "error" in error) {
    return (error as { error: string }).error;
  }
  return "An unknown error occurred";
}
