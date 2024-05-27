import { configureStore, createListenerMiddleware } from "@reduxjs/toolkit";
import { TypedUseSelectorHook, useDispatch, useSelector } from "react-redux";
import bookingReducer from "~/features/booking/booking.slice";
import commonReducer from "~/features/common/common.slice";
import movieReducer from "~/features/movie/movie.slice";
import notificationReducer from "~/features/notification/notification.slice";
import showtimeReducer from "~/features/showtime/showtime.slice";
import userReducer from "~/features/user/user.slice";

const listenerMiddleware = createListenerMiddleware();

const store = configureStore({
  reducer: {
    common: commonReducer,
    user: userReducer,
    movie: movieReducer,
    showtime: showtimeReducer,
    booking: bookingReducer,
    notification: notificationReducer,
  },
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware().prepend(listenerMiddleware.middleware),
});

export default store;
export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
export const useAppDispatch = () => useDispatch<AppDispatch>();
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;
