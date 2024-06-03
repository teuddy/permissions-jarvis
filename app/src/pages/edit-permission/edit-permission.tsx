import { Box, Button, Grid, MenuItem, Select, SelectChangeEvent, TextField, Typography } from "@mui/material";
import React, { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import { RootState, useAppDispatch } from "../../state/store";
import { Permission, PermissionType, UpdatePermissionRequest } from "../../models/permission";
import { get } from "lodash";
import { RequestState } from "../../models/action-state";
import { getAllPermissionTypes } from "../../state/reducers/permission-types.slice";
import { getPermissionById, updatePermission } from "../../state/reducers/permissions.slice";
import { useNavigate, useParams } from "react-router-dom";

const permissionTypeToSelect = (permissionTypeId: number) => {
    if (permissionTypeId === 0) return '';
    return permissionTypeId.toString();
}

const AddPermissionPage = () => {
    const params = useParams();
    const navigate = useNavigate();
    const dispatch = useAppDispatch();
    const permissionTypes = useSelector((state: RootState) => state.permissionType.permissionTypes);
    const requestState = useSelector((state: RootState) => state.permissionType.permissionTypesRequestState);

    const [model, setModel] = useState({} as UpdatePermissionRequest);

    const handleFormSubmit = (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        dispatch(updatePermission(model)).unwrap().then(() => {
            navigate('/permissions');
        });
    };
    
    const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = event.target;
        setModel({
            ...model,
            [name]: value
        });
    }

    useEffect(() => {
        if(requestState === RequestState.IDLE)
            dispatch(getAllPermissionTypes());
    },[dispatch, requestState]);

    useEffect(() => {
        const id = Number(params.id);
            dispatch(getPermissionById(id)).unwrap().then((permission: Permission) => {
                setModel({
                    id: get(permission, 'id', 0),
                    employeeForename: get(permission, 'employeeForename', ''),
                    employeeSurname: get(permission, 'employeeSurname', ''),
                    permissionType: get(permission, 'permissionType.id', 0),
                });
            });
    },[]);

    return (
        <React.Fragment>
            <Typography variant="h4">Request Permission</Typography>
           <Box component="form" onSubmit={handleFormSubmit}>
            <Grid container spacing={2} sx={{ mt: 3 }}>
                <Grid item xs={12} sm={6}>
                    <TextField name="employeeForename" value={get(model, 'employeeForename', '')} onChange={handleInputChange} label="Employee Forename" fullWidth required />
                </Grid>
                <Grid item xs={12} sm={6}>
                    <TextField name="employeeSurname" label="Employee Surname" value={get(model, 'employeeSurname', '')} onChange={handleInputChange} fullWidth required />
                </Grid>
                <Grid item xs={12}>
                    <Select
                        fullWidth
                        name="permissionTypeId"
                        required
                        value={permissionTypeToSelect(get(model, 'permissionType', 0))}
                        onChange={(event: SelectChangeEvent) => {
                            setModel({
                                ...model,
                                permissionType: Number(event.target.value)
                            });
                        }}
                        label="Permission Type"
                    >
                        {
                            permissionTypes.map((permissionType: PermissionType) => (
                                <MenuItem key={get(permissionType, 'id')} value={get(permissionType, 'id')}>{ get(permissionType, 'description')}</MenuItem>)
                                )
                        }
                    </Select>
                </Grid>
            </Grid>
            <Button
              type="submit"
              fullWidth
              variant="contained"
              sx={{ mt: 3, mb: 2 }}
            >
              Update Permission
            </Button>
           </Box>
        </React.Fragment>
    );
};

export default AddPermissionPage;