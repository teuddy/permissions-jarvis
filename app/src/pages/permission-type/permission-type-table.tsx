import { Table, TableBody, TableCell, TableHead, TableRow } from "@mui/material"
import { PermissionType } from "../../models/permission";
import { get } from "lodash";
import { useSelector } from "react-redux";
import { RootState } from "../../state/store";

const PermissionsTypeTable = () => {
    const permissionTypes = useSelector((state: RootState) => state.permissionType.permissionTypes);
    
    return(
        <Table size="medium">
        <TableHead>
          <TableRow>
            <TableCell>Description</TableCell>
            
          </TableRow>
        </TableHead>
        <TableBody>
          {permissionTypes.map((permissionType: PermissionType, index: number) => (
            <TableRow key={get(permissionType, 'id', index)}>
              <TableCell>{get(permissionType, 'description')}</TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    )
}

export default PermissionsTypeTable;