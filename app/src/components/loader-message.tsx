import { Box, CircularProgress, Typography } from "@mui/material";

const Loader = () => {
    return (
        <Box sx={{ display: 'flex' }}>
            <CircularProgress />
            <Typography variant="h5" align="center" color="text.primary" gutterBottom>
                Loading...
            </Typography>
        </Box>
    )
}

export default Loader;
