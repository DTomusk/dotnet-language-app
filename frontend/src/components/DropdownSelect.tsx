import { FormControl, MenuItem, Select, type SelectChangeEvent } from "@mui/material";

export type DropdownSelectItem = {
    value: string;
    label: string;
};

type DropdownSelectProps = {
    value: string;
    onChange: (value: string) => void;
    items: DropdownSelectItem[];
    placeholder: string;
};

export default function DropdownSelect({ value, onChange, items, placeholder }: DropdownSelectProps) {
    const handleChange = (event: SelectChangeEvent<string>) => {
        onChange(event.target.value);
    };

    return (
        <FormControl fullWidth variant="outlined">
            <Select
                value={value}
                onChange={handleChange}
                displayEmpty
                sx={{
                    borderRadius: 2,
                    backgroundColor: "background.paper",
                }}
                renderValue={(selected) => {
                    if (!selected) {
                        return <span style={{ color: "rgba(0, 0, 0, 0.6)" }}>{placeholder}</span>;
                    }

                    return items.find((item) => item.value === selected)?.label ?? selected;
                }}
            >
                <MenuItem value="" disabled>
                    {placeholder}
                </MenuItem>
                {items.map((item) => (
                    <MenuItem key={item.value} value={item.value}>
                        {item.label}
                    </MenuItem>
                ))}
            </Select>
        </FormControl>
    );
}