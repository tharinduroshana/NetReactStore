import { Elements } from "@stripe/react-stripe-js";
import CheckoutPage from "./CheckoutPage";
import { loadStripe } from "@stripe/stripe-js";
import { useAppDispatch } from "../../app/store/configureStore";
import { useEffect, useState } from "react";
import agent from "../../app/api/agent";
import { setBasket } from "../basket/basketSlice";
import LoadingComponent from "../../app/layout/LoadingComponent";

const stripePromise = loadStripe(
  "pk_test_51Of5ovKAZfEcvG9RruxqH1QQxFwrkaDfVpNWG0EC2jnBPoE1YvzLmk9VM6RAuI199bIjW4AR3nLnI3dLupknn4uR00IKpTb67u",
);

const CheckoutWrapper = () => {
  const dispatch = useAppDispatch();
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    agent.Payments.createPaymentIntent()
      .then((basket) => dispatch(setBasket(basket)))
      .catch((error) => console.log(error))
      .finally(() => setLoading(false));
  }, [dispatch]);

  if (loading) return <LoadingComponent message="Loading checkout" />;

  return (
    <Elements stripe={stripePromise}>
      <CheckoutPage />
    </Elements>
  );
};

export default CheckoutWrapper;
