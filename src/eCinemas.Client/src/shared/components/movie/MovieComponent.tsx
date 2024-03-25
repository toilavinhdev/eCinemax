import { View, Text, Image } from "react-native";
import React from "react";
import { IMovieViewModel } from "~/features/movie";

interface Props {
  movie: IMovieViewModel;
  onPress?: () => void;
}

const MovieComponent = (props: Props) => {
  const { movie, onPress } = props;
  return (
    <View>
      <Image source={{ uri: movie.poster }} width={200} height={200} />
      <Text className="text-white">{movie.title}</Text>
    </View>
  );
};

export default MovieComponent;
