import { Box, Button, Grid, TextField, Typography } from "@mui/material";
import React, { useState } from "react";
import { useAppDispatch } from "../../state/store";
import { PermissionTypeRequest } from "../../models/permission";
import { createPermissionType } from "../../state/reducers/permission-types.slice";
import { useNavigate } from "react-router-dom";

const AddPermissionTypePage = () => {
    const navigate = useNavigate();
    const dispatch = useAppDispatch();

    const [model, setModel] = useState({} as PermissionTypeRequest);

    const handleFormSubmit = (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        dispatch(createPermissionType(model)).unwrap().then(() => {
            navigate('/permission-types');
        });
    };
    
    const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = event.target;
        setModel({
            ...model,
            [name]: value
        });
    }

    return (
        <React.Fragment>
            <Typography variant="h4">Request Permission</Typography>
           <Box component="form" onSubmit={handleFormSubmit}>
            <Grid container spacing={2} sx={{ mt: 3 }}>
                <Grid item xs={12} sm={6}>
                    <TextField name="description" onChange={handleInputChange} label="Description" fullWidth required />
                </Grid>
            </Grid>
            <Button
              type="submit"
              fullWidth
              variant="contained"
              sx={{ mt: 3, mb: 2 }}
            >
              Create Permission Type
            </Button>
           </Box>
        </React.Fragment>
    );
};

export default AddPermissionTypePage;