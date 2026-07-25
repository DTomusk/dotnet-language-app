import { useNavigate } from "react-router-dom";
import AuthForm from "../features/auth/components/AuthForm";
import { useLogin } from "../features/auth/hooks/useLogin";
import { useAuth } from "../features/auth/hooks/useAuth";
import { useEffect } from "react";
import type { LoginSchema } from "../features/auth/schemas/loginSchema";

export default function LoginPage() {
    const navigate = useNavigate();
    const mutation = useLogin();
    const { logIn, isAuthenticated } = useAuth();

    useEffect(() => {
        if (isAuthenticated) {
            navigate("/", { replace: true });
        }
    }, [isAuthenticated, navigate]);

    const onSubmit = async (formData: LoginSchema) => {
        const response = await mutation.mutateAsync(formData);
        await logIn(response.token);
        navigate("/", { replace: true });
    }

    return (
        <AuthForm mode="login" onSubmit={onSubmit} />
    )
}