import TextField from "@mui/material/TextField";
import { useController, UseControllerProps } from "react-hook-form";

interface AppTextInputProps extends UseControllerProps {
  label: string;
}

const AppTextInput = (props: AppTextInputProps) => {
  const { fieldState, field } = useController({ ...props, defaultValue: "" });
  return (
    <TextField
      {...props}
      {...field}
      fullWidth
      variant="outlined"
      error={!!fieldState.error}
      helperText={fieldState.error?.message}
    />
  );
};

export default AppTextInput;
