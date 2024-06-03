import ErrorIcon from '@mui/icons-material/Error';
import { Grid, Typography } from "@mui/material";
import React from 'react';

const ErrorMessage = ({message} : {message: string}) => {
    return (
        <React.Fragment>
            <Grid container spacing={2} direction='row'>
                <Grid item xs={12}>
                    <Typography variant='h4' fontWeight='bold' gutterBottom>
                        <ErrorIcon color='error' fontSize='large' />{' '}
                        {message}
                    </Typography>
                </Grid>
            </Grid>
        </React.Fragment>
    )
};

export default ErrorMessage;