import { NextRequest, NextResponse } from "next/server";

export async function POST(request: NextRequest) {
  try {
    const body = await request.json();
    console.log("Received group data:", body);

    return NextResponse.json({ success: true, data: body });

    // Docelowy kod:
    // const response = await apiClient.post("");
    // if (!response.ok) throw new Error("Failed to post group data");
    // const data = await response.json();
    // return NextResponse.json(data);
  } catch (error) {
    console.error("Error posting groups:", error);
    return NextResponse.json(
      { error: "Failed to post group data" },
      { status: 500 },
    );
  }
}

let groups = [
  { id: "1", name: "Grupa 1", courses: ["Matematyka", "Fizyka"] },
  { id: "2", name: "Grupa 2", courses: ["Chemia", "Biologia"] },
];

export async function GET() {
  return NextResponse.json(groups);
}
