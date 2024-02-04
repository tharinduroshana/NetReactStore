import Typography from "@mui/material/Typography";
import React from "react";
import BasketTable from "../basket/BasketTable";
import { Grid } from "@mui/material";
import BasketSummary from "../basket/BasketSummary";
import { useAppSelector } from "../../app/store/configureStore";

const Review = () => {
  const { basket } = useAppSelector((state) => state.basket);

  return (
    <>
      <Typography variant="h6" gutterBottom>
        Order Summary
      </Typography>
      {basket && <BasketTable items={basket.items} isBasket={false} />}
      <Grid container>
        <Grid item xs={6} />
        <Grid item xs={6}>
          <BasketSummary />
        </Grid>
      </Grid>
    </>
  );
};

export default Review;
