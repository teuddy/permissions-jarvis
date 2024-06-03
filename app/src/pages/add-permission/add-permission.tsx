import { Box, Button, Grid, MenuItem, Select, SelectChangeEvent, TextField, Typography } from "@mui/material";
import React, { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import { RootState, useAppDispatch } from "../../state/store";
import { PermissionRequest, PermissionType } from "../../models/permission";
import { get } from "lodash";
import { RequestState } from "../../models/action-state";
import { getAllPermissionTypes } from "../../state/reducers/permission-types.slice";
import { requestPermission } from "../../state/reducers/permissions.slice";
import { useNavigate } from "react-router-dom";



const AddPermissionPage = () => {
    const navigate = useNavigate();
    const dispatch = useAppDispatch();
    const permissionTypes = useSelector((state: RootState) => state.permissionType.permissionTypes);
    const requestState = useSelector((state: RootState) => state.permissionType.permissionTypesRequestState);

    const [model, setModel] = useState({} as PermissionRequest);

    const handleFormSubmit = (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        dispatch(requestPermission(model)).unwrap().then(() => {
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



    return (
        <React.Fragment>
            <Typography variant="h4">Request Permission</Typography>
           <Box component="form" onSubmit={handleFormSubmit}>
            <Grid container spacing={2} sx={{ mt: 3 }}>
                <Grid item xs={12} sm={6}>
                    <TextField name="employeeForename" onChange={handleInputChange} label="Employee Forename" fullWidth required />
                </Grid>
                <Grid item xs={12} sm={6}>
                    <TextField name="employeeSurname" label="Employee Surname" onChange={handleInputChange} fullWidth required />
                </Grid>
                <Grid item xs={12}>
                    <Select
                        fullWidth
                        name="permissionTypeId"
                        required
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
                                <MenuItem value={permissionType.id}>{ get(permissionType, 'description')}</MenuItem>)
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
              Request Permission
            </Button>
           </Box>
        </React.Fragment>
    );
};

export default AddPermissionPage;