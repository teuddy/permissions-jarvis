import { IconButton, Table, TableBody, TableCell, TableHead, TableRow } from "@mui/material"
import { Permission } from "../../models/permission";
import { get, isEmpty } from "lodash";
import { useSelector } from "react-redux";
import { RootState } from "../../state/store";
import EditIcon from '@mui/icons-material/Edit';
import { useNavigate } from "react-router-dom";

export const dateFormat = (date: string): string => {
    if (isEmpty(date)) return '';
  
    const importedDate = new Date(date);
    return new Intl.DateTimeFormat('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
    }).format(importedDate);
  };

const PermissionsTable = () => {
    const navigate = useNavigate();
    const permissions = useSelector((state: RootState) => state.permission.permissions);
    
    return(
        <Table size="medium">
        <TableHead>
          <TableRow>
            <TableCell>Employee Forename</TableCell>
            <TableCell>Employee Surname</TableCell>
            <TableCell>Permission Type</TableCell>
            <TableCell>Granted On</TableCell>
            <TableCell></TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {permissions.map((permission: Permission, index: number) => (
            <TableRow key={get(permission, 'id', index)}>
              <TableCell>{get(permission, 'employeeForename')}</TableCell>
              <TableCell>{get(permission, 'employeeSurname')}</TableCell>
              <TableCell>{get(permission, 'permissionType.description')}</TableCell>
              <TableCell>{dateFormat(get(permission, 'grantedOn'))}</TableCell>
              <TableCell>
                <IconButton aria-label="delete" onClick={() => { navigate(`${get(permission, 'id')}`); }}>
                    <EditIcon />
                </IconButton>
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    )
}

export default PermissionsTable;