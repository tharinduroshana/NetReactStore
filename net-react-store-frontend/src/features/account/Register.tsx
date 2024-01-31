import * as React from "react";
import Avatar from "@mui/material/Avatar";
import TextField from "@mui/material/TextField";
import Grid from "@mui/material/Grid";
import Box from "@mui/material/Box";
import LockOutlinedIcon from "@mui/icons-material/LockOutlined";
import Typography from "@mui/material/Typography";
import Container from "@mui/material/Container";
import {
  Alert,
  AlertTitle,
  List,
  ListItem,
  ListItemText,
  Paper,
} from "@mui/material";
import { Link, useNavigate } from "react-router-dom";
import { useForm } from "react-hook-form";
import { LoadingButton } from "@mui/lab";
import agent from "../../app/api/agent";
import { toast } from "react-toastify";

export default function Register() {
  const navigate = useNavigate();
  const [validationErrors, setValidationErrors] = React.useState<any>(null);
  const {
    register,
    handleSubmit,
    formState: { isSubmitting, errors, isValid },
  } = useForm({ mode: "onTouched" });

  return (
    <Container
      component={Paper}
      maxWidth="sm"
      sx={{
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        p: 4,
      }}
    >
      <Avatar sx={{ m: 1, bgcolor: "secondary.main" }}>
        <LockOutlinedIcon />
      </Avatar>
      <Typography component="h1" variant="h5">
        Register
      </Typography>
      <Box
        component="form"
        onSubmit={handleSubmit((data) => {
          agent.Account.register(data)
            .then((response) => {
              toast.success("Registration successful!");
              navigate("/login");
            })
            .catch((error) => {
              console.log(error);
              setValidationErrors(error);
            });
        })}
        noValidate
        sx={{ mt: 1 }}
      >
        <TextField
          margin="normal"
          fullWidth
          id="username"
          label="Username"
          {...register("username", { required: "Username is required!" })}
          error={!!errors.username}
          helperText={errors?.username?.message as string}
          autoFocus
        />
        <TextField
          margin="normal"
          fullWidth
          id="name"
          label="Name"
          {...register("name", { required: "Full name is required!" })}
          error={!!errors.name}
          helperText={errors?.name?.message as string}
        />
        <TextField
          margin="normal"
          fullWidth
          label="Password"
          type="password"
          id="password"
          {...register("password", { required: "Password is required!" })}
          error={!!errors.password}
          helperText={errors?.password?.message as string}
        />
        {validationErrors && (
          <Alert severity="error">
            <AlertTitle>Validation Error</AlertTitle>
            <List>
              <ListItem key={validationErrors.data}>
                <ListItemText>{validationErrors.data}</ListItemText>
              </ListItem>
            </List>
          </Alert>
        )}
        <LoadingButton
          disabled={!isValid}
          loading={isSubmitting}
          type="submit"
          fullWidth
          variant="contained"
          sx={{ mt: 3, mb: 2 }}
        >
          Register
        </LoadingButton>
        <Grid container>
          <Grid item xs>
            <Link to="#">Forgot password?</Link>
          </Grid>
          <Grid item>
            <Link to="/login">{"Already have an account? Login"}</Link>
          </Grid>
        </Grid>
      </Box>
    </Container>
  );
}
