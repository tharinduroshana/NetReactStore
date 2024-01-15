import { debounce, TextField } from "@mui/material";
import React, { useCallback, useState } from "react";
import { useAppDispatch, useAppSelector } from "../../app/store/configureStore";
import { setProductParams } from "./catalogSlice";

const ProductSearch = () => {
  const { productParams } = useAppSelector((state) => state.catalog);
  const [searchTerm, setSearchTerm] = useState(productParams.searchTerm || "");
  const dispatch = useAppDispatch();

  const debounceSearch = useCallback(
    debounce((event: any) => {
      dispatch(setProductParams({ searchTerm: event.target.value }));
    }, 1000),
    [],
  );

  return (
    <TextField
      label="Seach Products"
      variant="outlined"
      fullWidth
      value={searchTerm || ""}
      onChange={(e: any) => {
        setSearchTerm(e.target.value);
        debounceSearch(e);
      }}
    />
  );
};

export default ProductSearch;
