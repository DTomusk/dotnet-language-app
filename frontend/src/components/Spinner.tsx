import { CircularProgress } from "@mui/material";

type SpinnerProps = {
    ariaLabel?: string;
};

export default function Spinner({ ariaLabel = "Loading…" }: SpinnerProps) {
    return (
        <CircularProgress aria-label={ariaLabel} />
    );
}