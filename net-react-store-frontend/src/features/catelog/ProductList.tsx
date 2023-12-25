import { Product } from "../../app/models/products";
import { Grid } from "@mui/material";
import React from "react";
import ProductCard from "./ProductCard";

type ProductListProps = {
  products: Product[];
};

const ProductList = ({ products }: ProductListProps) => {
  return (
    <>
      <Grid container spacing={4}>
        {products.map((item: Product) => (
          <Grid item xs={3} key={item.id}>
            <ProductCard product={item} />
          </Grid>
        ))}
      </Grid>
    </>
  );
};

export default ProductList;
