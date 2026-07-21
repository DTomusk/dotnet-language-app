import { Alert, Box, Button, Link, Stack, TextField, Typography } from "@mui/material";
import { useState } from "react";
import { Controller, useForm } from "react-hook-form";
import { useTranslation } from "react-i18next";
import { Link as RouterLink } from "react-router-dom";

type AuthMode = "login" | "register";

type AuthFormValues = {
  email: string;
  password: string;
  confirmPassword: string;
};

type AuthFormProps = {
  mode: AuthMode;
  onSubmit?: (values: { email: string; password: string }) => Promise<void> | void;
};

export default function AuthForm({ mode, onSubmit }: AuthFormProps) {
  const [submitError, setSubmitError] = useState<string | null>(null);
  const { t } = useTranslation(["auth", "common"]);

  const {
    control,
    handleSubmit,
    formState: { errors, isSubmitting },
    setError,
  } = useForm<AuthFormValues>({
    defaultValues: {
      email: "",
      password: "",
      confirmPassword: "",
    },
  });

  const isRegister = mode === "register";

  const submitLabel = isRegister ? t("auth:actions.submitRegister") : t("auth:actions.submitLogin");
  const title = isRegister ? t("auth:title.register") : t("auth:title.login");
  const subtitle = isRegister
    ? t("auth:subtitle.register")
    : t("auth:subtitle.login");

  async function onFormSubmit(values: AuthFormValues) {
    setSubmitError(null);

    if (isRegister && values.password !== values.confirmPassword) {
      setError("confirmPassword", {
        type: "validate",
        message: t("auth:validation.passwordsDoNotMatch"),
      });
      return;
    }

    try {
      if (onSubmit) {
        await onSubmit({ email: values.email, password: values.password });
      } else {
        await new Promise((resolve) => setTimeout(resolve, 500));
      }
    } catch (error) {
      const message =
        error instanceof Error ? error.message : t("common:errors.genericSubmit");
      setSubmitError(message);
    }
  }

  return (
    <Box component="section">
      <Stack spacing={3}>
        <Stack spacing={1}>
          <Typography variant="h5">{title}</Typography>
          <Typography variant="body2" color="text.secondary">
            {subtitle}
          </Typography>
        </Stack>

        {submitError ? <Alert severity="error">{submitError}</Alert> : null}

        <Stack component="form" spacing={2} onSubmit={handleSubmit(onFormSubmit)} noValidate>
          <Controller
            name="email"
            control={control}
            rules={{
              required: t("auth:validation.emailRequired"),
              pattern: {
                value: /^[^\s@]+@[^\s@]+\.[^\s@]+$/,
                message: t("auth:validation.emailInvalid"),
              },
            }}
            render={({ field }) => (
              <TextField
                {...field}
                label={t("auth:fields.email")}
                type="email"
                autoComplete="email"
                error={!!errors.email}
                helperText={errors.email?.message}
                fullWidth
              />
            )}
          />

          <Controller
            name="password"
            control={control}
            rules={{
              required: t("auth:validation.passwordRequired"),
              minLength: {
                value: 8,
                message: t("auth:validation.passwordMinLength"),
              },
            }}
            render={({ field }) => (
              <TextField
                {...field}
                label={t("auth:fields.password")}
                type="password"
                autoComplete={isRegister ? "new-password" : "current-password"}
                error={!!errors.password}
                helperText={errors.password?.message}
                fullWidth
              />
            )}
          />

          {isRegister ? (
            <Controller
              name="confirmPassword"
              control={control}
              rules={{
                required: t("auth:validation.confirmPasswordRequired"),
              }}
              render={({ field }) => (
                <TextField
                  {...field}
                  label={t("auth:fields.confirmPassword")}
                  type="password"
                  autoComplete="new-password"
                  error={!!errors.confirmPassword}
                  helperText={errors.confirmPassword?.message}
                  fullWidth
                />
              )}
            />
          ) : null}

          <Button type="submit" variant="contained" size="large" disabled={isSubmitting}>
            {isSubmitting ? t("common:actions.submitting") : submitLabel}
          </Button>
        </Stack>

        <Typography variant="body2" color="text.secondary">
          {isRegister ? t("auth:prompts.alreadyHaveAccount") : t("auth:prompts.newHere")}
          <Link
            component={RouterLink}
            to={isRegister ? "/auth/login" : "/auth/register"}
            underline="hover"
            sx={{ fontWeight: 600 }}
          >
            {isRegister ? t("auth:actions.switchToLogin") : t("auth:actions.switchToRegister")}
          </Link>
        </Typography>
      </Stack>
    </Box>
  );
}