import { Button, Grid, Typography } from "@mui/material";
import { useNavigate } from "react-router-dom";
import React from "react";
import PermissionTypeBody from "./permission-type-body";

const PermissionTypesPage = () => {    
    const navigate = useNavigate();
    return (
        <React.Fragment>
            <Grid container spacing={2}>
                <Grid item xs={12}>
                    <Typography variant="h3">Permission Types</Typography>
                </Grid>
                <Grid item xs={12}>
                    <Button variant="contained" color="primary" onClick={() => navigate('create')}>Add Permission Type</Button>
                </Grid>
                <Grid item xs={12}>
                    <PermissionTypeBody />
                </Grid>
            </Grid>
        </React.Fragment>
    );
};

export default PermissionTypesPage;