import { NextResponse } from "next/server";

let groupSets = [
  {
    id: "1",
    name: "Grupy Semestr Letni 2024/2025",
    createdAt: "2025-01-01",
    groups: [
      { id: "101", name: "Grupa A", courses: ["Matematyka", "Fizyka"] },
      { id: "102", name: "Grupa B", courses: ["Chemia"] },
    ],
  },
  {
    id: "2",
    name: "Grupy Semestr Zimowy 2024/2025",
    createdAt: "2025-01-02",
    groups: [
      { id: "201", name: "Grupa C", courses: ["Historia"] },
      { id: "202", name: "Grupa D", courses: ["Biologia"] },
    ],
  },
];

export async function GET(request: Request) {
  const { searchParams } = new URL(request.url);
  const groupId = searchParams.get("id");

  try {
    if (groupId) {
      const groupSet = groupSets.find((set) => set.id === groupId);

      if (!groupSet) {
        return NextResponse.json(
          { error: "Group set not found" },
          { status: 404 },
        );
      }

      return NextResponse.json(groupSet);

      /*
      const response = await apiClient.get(`/groups/${groupId}`);
      return NextResponse.json(response.data);
      */
    }

    return NextResponse.json(groupSets);

    /*
    const response = await apiClient.get("/groups");
    return NextResponse.json(response.data);
    */
  } catch (error) {
    console.error("Błąd podczas pobierania danych zestawów grup:", error);
    return NextResponse.json(
      { error: "Failed to fetch group sets" },
      { status: 500 },
    );
  }
}

export async function POST(request: Request) {
  try {
    const body = await request.json();
    const newGroupSet = {
      ...body,
      id: (groupSets.length + 1).toString(),
      createdAt: new Date().toISOString(),
    };

    groupSets.push(newGroupSet);
    return NextResponse.json(newGroupSet, { status: 201 });

    /*
    const response = await apiClient.post("/groups", body);
    return NextResponse.json(response.data, { status: response.status });
    */
  } catch (error) {
    console.error("Błąd podczas tworzenia zestawu grup:", error);
    return NextResponse.json(
      { error: "Failed to create group set" },
      { status: 500 },
    );
  }
}

export async function DELETE(request: Request) {
  try {
    const { searchParams } = new URL(request.url);
    const groupId = searchParams.get("id");

    if (!groupId) {
      return NextResponse.json(
        { error: "Missing group set ID" },
        { status: 400 },
      );
    }

    groupSets = groupSets.filter((set) => set.id !== groupId);
    return NextResponse.json({ success: true }, { status: 200 });

    /*
    const response = await apiClient.delete(`/groups/${groupId}`);
    if (response.status === 200) {
      return NextResponse.json({ success: true }, { status: 200 });
    } else {
      throw new Error("Failed to delete group set");
    }
    */
  } catch (error) {
    console.error("Błąd podczas usuwania zestawu grup:", error);
    return NextResponse.json(
      { error: "Failed to delete group set" },
      { status: 500 },
    );
  }
}

export async function PATCH(request: Request) {
  try {
    const body = await request.json();
    const { id, ...updates } = body;

    const groupSetIndex = groupSets.findIndex((set) => set.id === id);
    if (groupSetIndex === -1) {
      return NextResponse.json(
        { error: "Group set not found" },
        { status: 404 },
      );
    }

    groupSets[groupSetIndex] = {
      ...groupSets[groupSetIndex],
      ...updates,
    };

    return NextResponse.json(groupSets[groupSetIndex], { status: 200 });

    /*
    const response = await apiClient.patch(`/groups/${id}`, updates);
    return NextResponse.json(response.data, { status: response.status });
    */
  } catch (error) {
    console.error("Błąd podczas edytowania zestawu grup:", error);
    return NextResponse.json(
      { error: "Failed to edit group set" },
      { status: 500 },
    );
  }
}
