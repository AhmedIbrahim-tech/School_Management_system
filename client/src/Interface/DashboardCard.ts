import { Student } from "./Student";

export interface DashboardCardProps {
    title: string;
    count: number;
    icon: string;
    link: string;
    color: string;
  }

  export interface HomeState {
    students: Student[];
    loading: boolean;
    error: string;
    addModalShow: boolean;
    editModalShow: boolean;
    selectedStudent: Student | null;
  }