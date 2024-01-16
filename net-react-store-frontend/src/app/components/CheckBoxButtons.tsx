import { Checkbox, FormControlLabel, FormGroup } from "@mui/material";
import React from "react";

type CheckBoxButtonsProps = {
  items: string[];
  checked?: string[];
  onChange: (items: string[]) => void;
};

const CheckBoxButtons = ({
  items,
  checked,
  onChange,
}: CheckBoxButtonsProps) => {
  const [checkedItems, setCheckedItems] = React.useState(checked || []);

  const handleChecked = (value: string) => {
    const currentIndex = checkedItems.findIndex((item) => item === value);
    let newChecked: string[] = [];
    if (currentIndex === -1) newChecked = [...checkedItems, value];
    else newChecked = checkedItems.filter((item) => item !== value);
    setCheckedItems(newChecked);
    onChange(newChecked);
  };

  return (
    <FormGroup>
      {items.map((item) => (
        <FormControlLabel
          control={
            <Checkbox
              checked={checkedItems.indexOf(item) !== -1}
              onClick={() => handleChecked(item)}
            />
          }
          label={item}
          key={item}
        />
      ))}
    </FormGroup>
  );
};

export default CheckBoxButtons;
