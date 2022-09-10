{-# LANGUAGE LambdaCase #-}
import Data.List
import Data.Char
import System.IO
import Control.Monad.Writer
import Numeric (showHex, showFFloat)

data Color = Color Int Int Int deriving Show

readColor :: String -> Color
readColor (r1:r2:g1:g2:b1:b2:[]) = Color (foo r1 r2) (foo g1 g2) (foo b1 b2) where
    foo a b = (digitToInt a)*16 + (digitToInt b)

compToFloat :: Int -> Float
compToFloat comp = (/256.0) $ fromIntegral comp 

css :: [Color] -> Writer String ()
css palette = do
    tell "main {\n"
    forM palette $ \case
        Color r g b -> do
            tell "  "
            tell $ takeBack 2 $ ("0"++) $ showHex r ""
            tell $ takeBack 2 $ ("0"++) $ showHex g ""
            tell $ takeBack 2 $ ("0"++) $ showHex b ""
            tell "\n"
    tell "}\n"
    return ()

glslPalette :: [Color] -> String
glslPalette palette =
    ("vec3 palette[] = vec3[](\n"++) $
    (++"\n)") $
    intercalate ",\n" $
    map (\(Color r g b) ->
        ("  vec3("++) $
        (++")") $
        intercalate ", " $
        map showFloat [r,g,b]
    ) palette where
        showFloat f = showFFloat (Just 4) (compToFloat f) ""

main :: IO ()
main = do
    contents <- readFile "in.txt"
    let palette = fmap (readColor . takeBack 6) $ words $ contents
    let c = glslPalette palette
    putStrLn c
    putStrLn $ show $ length palette
    -- putStrLn contents

takeBack n str = reverse $ take n $ reverse str

-- vec3 alumi[] = vec3[](
--     vec3(0.223, 0.192, 0.294),
--     vec3(0.490, 0.439, 0.443),
--     vec3(0.627, 0.576, 0.556),
--     vec3(0.874, 0.964, 0.960)
-- );
