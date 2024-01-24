import { User } from "../../app/models/user";
import { createAsyncThunk, createSlice, isAnyOf } from "@reduxjs/toolkit";
import { FieldValues } from "react-hook-form";
import agent from "../../app/api/agent";
import { routes } from "../../app/router/Routes";
import { toast } from "react-toastify";

type AccountState = {
  user: User | null;
};

const initialState: AccountState = {
  user: null,
};

export const signInUser = createAsyncThunk<User, FieldValues>(
  "account/signInUser",
  async (data, thunkAPI) => {
    try {
      const user = await agent.Account.login(data);
      localStorage.setItem("user", JSON.stringify(user));
      return user;
    } catch (e: any) {
      return thunkAPI.rejectWithValue({ error: e.data });
    }
  },
);

export const fetchUser = createAsyncThunk<User>(
  "account/fetchUser",
  async (_, thunkAPI) => {
    thunkAPI.dispatch(setUser(JSON.parse(localStorage.getItem("user")!)));
    try {
      const username = JSON.parse(localStorage.getItem("user")!).username;
      const user = await agent.Account.fetchUser({ username });
      localStorage.setItem("user", JSON.stringify(user));
      return user;
    } catch (e: any) {
      return thunkAPI.rejectWithValue({ error: e.data });
    }
  },
  {
    condition: () => {
      const user = localStorage.getItem("user");
      if (!user) {
        return false;
      }
    },
  },
);

export const accountSlice = createSlice({
  name: "account",
  initialState,
  reducers: {
    signOut: (state) => {
      state.user = null;
      localStorage.removeItem("user");
      routes.navigate("/");
    },
    setUser: (state, action) => {
      state.user = action.payload;
    },
  },
  extraReducers: (builder) => {
    builder.addCase(fetchUser.rejected, (state) => {
      state.user = null;
      localStorage.removeItem("user");
      toast.error("Session Expired - Please login again");
      routes.navigate("/login");
    });
    builder.addMatcher(
      isAnyOf(signInUser.fulfilled, fetchUser.fulfilled),
      (state, action) => {
        state.user = action.payload;
      },
    );
    builder.addMatcher(isAnyOf(signInUser.rejected), (state, action) => {
      console.log(action.payload);
    });
  },
});

export const { signOut, setUser } = accountSlice.actions;
