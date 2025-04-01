export interface Event {
  id: string;
  title: string;
  description: string;
  startTime: string;
  endTime: string;
  room?: string;
  lecturer?: string;
}
export type Course = string;
export interface Group {
  groupId: number;
  groupName: string;
  assignedFieldsOfStudy: Course[];
  key: string;
}
export type BasicGroup = Omit<Group, "assignedFieldsOfStudy" | "key">;
export interface Semester {
  id: number;
  creationStage: number;
  academicYear: string;
  name: string;
  startDate: string;
  endDate: string;
  createDate: string;
  updateDate: string;
  courses: Course[];
  scheduleClassDays: [];
}

export interface GroupSet {
  id: string;
  name: string;
  createdAt: string;
  groups: Group[];
}
export interface User {
  id: number;
  roleId: number;
  roleName: string;
  email: string;
  firstName: string;
  lastName: string;
  phoneNumber: string | null;
}
export interface Weekend {
  date: string;
  availability: Record<string, boolean>;
}
