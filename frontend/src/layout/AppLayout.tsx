import Box from "@mui/material/Box";
import { Outlet } from "react-router-dom";

export default function AppLayout() {
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
            <Outlet />
        </Box>
    )
}