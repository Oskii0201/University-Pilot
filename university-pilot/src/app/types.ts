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
export interface FieldOfStudyGroup {
  groupId: number;
  groupName: string;
  assignedFieldsOfStudy: Course[];
}

export interface FieldsOfStudyAssignmentsResponse {
  semesterId: number;
  unassignedFieldsOfStudy: Course[];
  assignedFieldOfStudyGroups: FieldOfStudyGroup[];
}

export interface Group extends FieldOfStudyGroup {
  key: string;
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
