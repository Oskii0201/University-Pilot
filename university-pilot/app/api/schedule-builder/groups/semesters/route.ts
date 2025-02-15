import { NextResponse } from "next/server";
import apiClient from "@/app/lib/apiClient";
import { handleApiError } from "@/utils/handleApiError";

export async function GET() {
  try {
    const result = await apiClient.get("/StudyProgram/GetUpcomingSemesters");

    return NextResponse.json(result.data);
  } catch (error) {
    const errorMessage = handleApiError(error);
    return NextResponse.json({ error: errorMessage }, { status: 500 });
  }
}
