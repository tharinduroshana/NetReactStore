import * as yup from "yup";

export const validationSchema = [
  yup.object({
    fullName: yup.string().required("Full name is required"),
    address1: yup.string().required("Address Line 1 is required"),
    city: yup.string().required(),
    postalCode: yup.string().required(),
    country: yup.string().required(),
  }),
  yup.object(),
  yup.object({
    nameOnCard: yup.string().required(),
  }),
];
