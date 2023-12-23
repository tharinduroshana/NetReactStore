import { Product } from "../../app/models/products";
import {
  Button,
  Card,
  CardActions,
  CardContent,
  CardMedia,
  Typography,
} from "@mui/material";
import React from "react";

type ProductCardType = {
  product: Product;
};

const ProductCard = ({ product }: ProductCardType) => {
  return (
    <>
      <Card>
        <CardMedia
          sx={{ height: 140 }}
          image={product.pictureUrl}
          title="green iguana"
        />
        <CardContent>
          <Typography gutterBottom variant="h5" component="div">
            {product.name}
          </Typography>
          <Typography variant="body2" color="text.secondary">
            {product.description}
          </Typography>
        </CardContent>
        <CardActions>
          <Button size="small">Share</Button>
          <Button size="small">Learn More</Button>
        </CardActions>
      </Card>
    </>
  );
};

export default ProductCard;
