import { useController, UseControllerProps } from "react-hook-form";
import FormControlLabel from "@mui/material/FormControlLabel";
import Checkbox from "@mui/material/Checkbox";

interface AppCheckBoxProps extends UseControllerProps {
  label: string;
  disabled: boolean;
}

const AppCheckBox = (props: AppCheckBoxProps) => {
  const { field } = useController({ ...props, defaultValue: false });
  return (
    <FormControlLabel
      label={props.label}
      disabled={props.disabled}
      control={<Checkbox {...field} checked={field.value} color="secondary" />}
    />
  );
};

export default AppCheckBox;
