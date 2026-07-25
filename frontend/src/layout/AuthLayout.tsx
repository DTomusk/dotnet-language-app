import { Box, Container, Paper } from "@mui/material";
import { Outlet } from "react-router-dom";

export default function AuthLayout() {
    return (
        <Box
            sx={{
                display: 'flex',
                minHeight: '100dvh',
                alignItems: 'center',
                justifyContent: 'center',
                px: 2,
                py: 4,
                background: (theme) =>
                    `linear-gradient(135deg, ${theme.palette.primary.light}1f 0%, ${theme.palette.background.default} 45%, ${theme.palette.secondary.light}1a 100%)`,
            }}
        >
            <Container maxWidth="sm" disableGutters>
                <Paper
                    elevation={3}
                    sx={{
                        p: { xs: 3, sm: 4 },
                        borderRadius: 3,
                        border: '1px solid',
                        borderColor: 'divider',
                    }}
                >
                    <Outlet />
                </Paper>
            </Container>
        </Box>
    )
}