export interface Student {
    studID: number;
    name: string;
    address: string;
    phone: string;
    departmentName: string;
}


export interface CreateStudent {
    nameAr: string;
    nameEn: string;
    address: string;
    phone: string;
  }

  export interface UpdateStudent {
    id: number;
    nameAr: string;
    nameEn: string;
    address: string;
    phone: string;
  }
