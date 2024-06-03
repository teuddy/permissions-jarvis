export interface Permission {
    id: number;
    employeeForename: string;
    employeeSurname: string;
    permissionType: PermissionType;
    grantedOn: string
}

export interface PermissionType{
    id: number;
    description: string;
}

export interface PermissionRequest {
    employeeForename: string;
    employeeSurname: string;
    permissionType: number;
}

export interface UpdatePermissionRequest extends PermissionRequest {
    id: number;
}

export interface PermissionTypeRequest {
    description: string;
}