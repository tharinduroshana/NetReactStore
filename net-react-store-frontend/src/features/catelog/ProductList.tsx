import { Product } from "../../app/models/products";
import { Grid } from "@mui/material";
import React from "react";
import ProductCard from "./ProductCard";
import { useAppSelector } from "../../app/store/configureStore";
import ProductCardSkeleton from "./ProductCardSkeleton";

type ProductListProps = {
  products: Product[];
};

const ProductList = ({ products }: ProductListProps) => {
  const { productsLoaded } = useAppSelector((state) => state.catalog);
  return (
    <>
      <Grid container spacing={4}>
        {products.map((item: Product) => (
          <Grid item xs={4} key={item.id}>
            {!productsLoaded ? (
              <ProductCardSkeleton />
            ) : (
              <ProductCard product={item} />
            )}
          </Grid>
        ))}
      </Grid>
    </>
  );
};

export default ProductList;
