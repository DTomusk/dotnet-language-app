import { createContext, useEffect, useState, type ReactNode } from "react";
import { clearToken, setToken } from "../lib/auth/token";
import { registerUnauthorizedHandler } from "../lib/auth/session";

type AuthContextValue = {
    isAuthenticated: boolean;
    signIn: (token: string) => void;
    signOut: () => void;
};

export const AuthContext = createContext<AuthContextValue | null>(null);

export function AuthProvider({ children }: { children: ReactNode }) {
    const [isAuthenticated, setIsAuthenticated] = useState(false);

    const signIn = (token: string) => {
        setToken(token);
        setIsAuthenticated(true);
    };

    const signOut = () => {
        clearToken();
        setIsAuthenticated(false);
    };

    useEffect(() => {
        registerUnauthorizedHandler(signOut);

        return () => {
            registerUnauthorizedHandler(null);
        };
    }, [signOut]);

    return (
        <AuthContext.Provider value={{ isAuthenticated, signIn, signOut }}>
            {children}
        </AuthContext.Provider>
    );
} 