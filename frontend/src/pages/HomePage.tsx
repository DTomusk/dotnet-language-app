import { Button, Stack, Typography } from "@mui/material";
import { useNavigate } from "react-router-dom";

export default function HomePage() {
    const navigate = useNavigate();

    return (
        <Stack spacing={5} 
            sx={{
                maxWidth: 600,
                width: "100%",
                textAlign: "center",
            }}>
            <Typography variant="h3" component="h1">
                Welcome back DisplayName
            </Typography>
            <Button
                variant="contained"
                size="large"
                color="primary"
                sx={{ alignSelf: "center" }}
                onClick={() => navigate("/practice")}
            >
                Start Practicing
            </Button>
        </Stack>
    )
}