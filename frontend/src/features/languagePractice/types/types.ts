export type SubmissionRequest = {
    text: string;
};

export type Language = {
    languageCode: string;
    languageName: string;
};

export type SetLanguageRequest = {
    languageCode: string;
};

export type GetLanguageStatsResponse = {
    displayName: string;
    uniqueLemmas: number;
    daysPractised: number;
};