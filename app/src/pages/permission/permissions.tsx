import { Button, Grid, Typography } from "@mui/material";
import { useNavigate } from "react-router-dom";
import React from "react";
import PermissionsBody from "./permissions-body";

const PermissionsPage = () => {
    const navigate = useNavigate();

    return (
        <React.Fragment>
            <Grid container spacing={2}>
                <Grid item xs={12}>
                    <Typography variant="h3">Permissions</Typography>
                </Grid>
                <Grid item xs={12}>
                    <Button variant="contained" color="primary" onClick={() => navigate('/permissions/request')}>Add Permission</Button>
                </Grid>
                <Grid item xs={12}>
                    <PermissionsBody />
                </Grid>
            </Grid>
        </React.Fragment>
    );
};

export default PermissionsPage;