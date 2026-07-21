import { Alert, Box, Button, Paper, Stack, TextField, Typography } from "@mui/material";
import { useTranslation } from "react-i18next";
import type { SubmissionSchema } from "../schemas/submissionSchema";
import { Controller, useForm } from "react-hook-form";
import { useState } from "react";

type SubmissionFormProps = {
    onSubmit: (values: SubmissionSchema) => Promise<void> | void;
};

export default function SubmissionForm({ onSubmit }: SubmissionFormProps) {
    const [submitError, setSubmitError] = useState<string | null>(null);
    const { t } = useTranslation(["languagePractice", "common"]);

    const {
        control,
        handleSubmit,
        formState: { errors, isSubmitting },
    } = useForm<SubmissionSchema>({
        defaultValues: {
            text: "",
        },
    });

    async function onFormSubmit(values: SubmissionSchema) {
        setSubmitError(null);

        try {
            if (onSubmit) {
                await onSubmit({ text: values.text });
            } else {
                await new Promise((resolve) => setTimeout(resolve, 500));
            }
        } catch (error: any) {
            const message =
                error instanceof Error ? error.message : t("common:errors.genericSubmit");
            setSubmitError(message);
        }
    }

    return (
        <Paper elevation={3} 
            sx={{
                p: 4, 
                margin: "0 auto",
                borderRadius: 3,
            }}>
            <Box>
                <Stack spacing={2}>
                    <Typography variant="h4">{t("languagePractice:submission.Title")}</Typography>

                    {submitError ? <Alert severity="error">{submitError}</Alert> : null}

                    <Stack component="form" onSubmit={handleSubmit(onFormSubmit)} spacing={2}>
                        <Controller
                            name="text"
                            control={control}
                            rules={{
                                required: t("languagePractice:submission.validation.textRequired"),
                                maxLength: {
                                    value: 500,
                                    message: t("languagePractice:submission.validation.textMaxLength"),
                                },
                            }}
                            render={({ field }) => (
                                <TextField
                                    {...field}
                                    label={t("languagePractice:submission.fields.text")}
                                    multiline
                                    rows={4}
                                    error={!!errors.text}
                                    helperText={errors.text?.message}
                                    fullWidth
                                />
                            )}
                        />

                        <Button type="submit" variant="contained" color="primary" disabled={isSubmitting}>
                            {t("common:actions.submit")}
                        </Button>
                    </Stack>
                </Stack>
            </Box>
        </Paper>
    );
}