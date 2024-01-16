import {
  FormControl,
  FormControlLabel,
  Radio,
  RadioGroup,
} from "@mui/material";
import React from "react";

type RadioButtonGroupProps = {
  options: any[];
  onChange: (event: any) => void;
  selectedValue: string;
};

const RadioButtonGroup = ({
  options,
  selectedValue,
  onChange,
}: RadioButtonGroupProps) => {
  return (
    <FormControl>
      <RadioGroup onChange={onChange} value={selectedValue}>
        {options.map(({ value, label }) => (
          <FormControlLabel
            value={value}
            control={<Radio />}
            label={label}
            key={label}
          />
        ))}
      </RadioGroup>
    </FormControl>
  );
};

export default RadioButtonGroup;
