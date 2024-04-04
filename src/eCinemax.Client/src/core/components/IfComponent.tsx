import { ReactNode } from "react";

interface Props {
  condition: boolean;
  children: ReactNode;
  elseTemplate?: ReactNode;
}

const IfComponent = (props: Props) => {
  const { condition, children, elseTemplate = null } = props;

  if (condition) return children;
  return elseTemplate;
};

export default IfComponent;
