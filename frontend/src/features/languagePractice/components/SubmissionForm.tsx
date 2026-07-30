import { Box, Button, Paper, Stack, TextField, Typography } from "@mui/material";
import { useTranslation } from "react-i18next";
import type { SubmissionSchema } from "../schemas/submissionSchema";
import { Controller, useForm } from "react-hook-form";
import { useEffect, useRef } from "react";

type SubmissionFormProps = {
    onSubmit: (values: SubmissionSchema) => Promise<void> | void;
    isSubmitting?: boolean;
    isSuccess?: boolean;
};

export default function SubmissionForm({ onSubmit, isSubmitting, isSuccess }: SubmissionFormProps) {
    const { t } = useTranslation(["languagePractice", "common"]);

    const {
        control,
        handleSubmit,
        reset,
        formState: { errors },
    } = useForm<SubmissionSchema>({
        defaultValues: {
            text: "",
        },
    });

    const prevIsSuccessRef = useRef(false);

    useEffect(() => {
    const justSucceeded = !prevIsSuccessRef.current && !!isSuccess;
    if (justSucceeded) {
        reset({ text: "" });
    }
    prevIsSuccessRef.current = !!isSuccess;
    }, [isSuccess, reset]);

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
                    <Stack component="form" onSubmit={handleSubmit(onSubmit)} spacing={2}>
                        <Controller
                            name="text"
                            control={control}
                            rules={{
                                required: t("languagePractice:submission.validation.textRequired"),
                                maxLength: {
                                    value: 500,
                                    message: t("languagePractice:submission.validation.textMaxLength"),
                                },
                                validate: (value) => {
                                    if (value.trim().length === 0) {
                                        return t("languagePractice:submission.validation.textRequired");
                                    }
                                    return true;
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

                        <Button type="submit" variant="contained" color="primary" disabled={isSubmitting ?? false}>
                            {t("common:actions.submit")}
                        </Button>
                    </Stack>
                </Stack>
            </Box>
        </Paper>
    );
}