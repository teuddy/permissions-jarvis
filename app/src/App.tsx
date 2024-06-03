import { createTheme, ThemeProvider } from '@mui/material/styles';
import './App.css'
import 'react-toastify/dist/ReactToastify.css';
import { AppBar, Container, CssBaseline, GlobalStyles, Link, Toolbar, Typography } from '@mui/material';
import { BrowserRouter } from 'react-router-dom';
import { ToastContainer } from 'react-toastify';
import Router from './router/router';

function App() {
  const defaultTheme = createTheme();
  return (
    <ThemeProvider theme={defaultTheme}>
      <GlobalStyles styles={{ ul: { margin: 0, padding: 0, listStyle: 'none' } }} />
      <CssBaseline />
      <BrowserRouter>
      <AppBar position='static' color='default' elevation={0} sx={{ borderBottom: `1px solid ${defaultTheme.palette.divider}` }}>
        <Toolbar sx={{ flexWrap: 'wrap' }}>
          <Typography variant='h6' color='inherit' noWrap sx={{ flexGrow: 1 }}>
            Permissions Challenge
          </Typography>
          <nav>
            <Link variant='button' color='text.primary' href='/permissions' sx={{ my: 1, mx: 1.5 }}>
              Permissions
            </Link>
            <Link variant='button' color='text.primary' href='/permission-types' sx={{ my: 1, mx: 1.5 }}>
              Permission Types
            </Link>
          </nav>
        </Toolbar>
      </AppBar>
      <Container maxWidth="lg" component="main" disableGutters sx={{ pt:8, pb: 6 }}>
          <Router />
          <ToastContainer />
      </Container>
      </BrowserRouter>
    </ThemeProvider>
  )
}

export default App
