import Typography from "@mui/material/Typography";
import Grid from "@mui/material/Grid";
import { useFormContext } from "react-hook-form";
import AppTextInput from "../../app/components/AppTextInput";
import AppCheckBox from "../../app/components/AppCheckBox";

const AddressForm = () => {
  const { control, formState } = useFormContext();

  return (
    <>
      <Typography variant="h6" gutterBottom>
        Shipping address
      </Typography>
      <Grid container spacing={3}>
        <Grid item xs={12} sm={12}>
          <AppTextInput label="Full Name" name="fullName" control={control} />
        </Grid>
        <Grid item xs={12}>
          <AppTextInput
            label="Address Line 1"
            name="address1"
            control={control}
          />
        </Grid>
        <Grid item xs={12}>
          <AppTextInput
            label="Address Line 2"
            name="address2"
            control={control}
          />
        </Grid>
        <Grid item xs={12} sm={6}>
          <AppTextInput label="City" name="city" control={control} />
        </Grid>
        <Grid item xs={12} sm={6}>
          <AppTextInput
            label="State/Province/Region"
            name="state"
            control={control}
          />
        </Grid>
        <Grid item xs={12} sm={6}>
          <AppTextInput
            label="Zip / Postal code"
            name="postalCode"
            control={control}
          />
        </Grid>
        <Grid item xs={12} sm={6}>
          <AppTextInput label="Country" name="country" control={control} />
        </Grid>
        <Grid item xs={12}>
          <AppCheckBox
            disabled={!formState.isDirty}
            label="Save this as default address"
            name="saveAddress"
            control={control}
          />
        </Grid>
      </Grid>
    </>
  );
};

export default AddressForm;
