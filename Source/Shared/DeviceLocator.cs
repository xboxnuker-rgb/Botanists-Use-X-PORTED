using Il2CppScheduleOne.EntityFramework;
using Il2CppScheduleOne.ObjectScripts;
using Il2CppScheduleOne.Tiles;

namespace BotanistsUseX.Runtime;

internal static class DeviceLocator
{
    private const int SearchRadius = 3;

#if FEATURE_SOIL
    internal static SoilPourer? FindSoilPourer(Pot pot)
    {
        foreach (Tile tile in EnumerateCardinalTiles(pot))
        {
            foreach (GridItem occupant in tile.BuildableOccupants)
            {
                if (occupant is null || !occupant.TryGetComponent(out SoilPourer pourer) || pourer is null)
                {
                    continue;
                }

                if (ContainsPot(pourer.GetPots(), pot))
                {
                    return pourer;
                }
            }
        }

        return null;
    }
#endif

#if FEATURE_SPRINKLER
    internal static Sprinkler? FindUsableSprinkler(Pot pot)
    {
        foreach (Tile tile in EnumerateCardinalTiles(pot))
        {
            foreach (GridItem occupant in tile.BuildableOccupants)
            {
                if (occupant is null || !occupant.TryGetComponent(out Sprinkler sprinkler) || sprinkler is null)
                {
                    continue;
                }

                if (ContainsPot(sprinkler.GetPots(), pot) && (sprinkler.IsSprinkling || sprinkler.CanWater()))
                {
                    return sprinkler;
                }
            }
        }

        return null;
    }
#endif

    private static IEnumerable<Tile> EnumerateCardinalTiles(Pot pot)
    {
        Coordinate origin = new(pot._originCoordinate);

        for (int distance = 1; distance <= SearchRadius; distance++)
        {
            Coordinate[] coordinates =
            {
                new(origin.x + distance, origin.y),
                new(origin.x - distance, origin.y),
                new(origin.x, origin.y + distance),
                new(origin.x, origin.y - distance),
            };

            foreach (Coordinate coordinate in coordinates)
            {
                Tile tile = pot.OwnerGrid.GetTile(coordinate);
                if (tile is not null)
                {
                    yield return tile;
                }
            }
        }
    }

    private static bool ContainsPot(Il2CppSystem.Collections.Generic.List<Pot> pots, Pot target)
    {
        Coordinate targetCoordinate = new(target._originCoordinate);

        foreach (Pot pot in pots)
        {
            if (pot is null)
            {
                continue;
            }

            Coordinate coordinate = new(pot._originCoordinate);
            if (coordinate.x == targetCoordinate.x && coordinate.y == targetCoordinate.y)
            {
                return true;
            }
        }

        return false;
    }
}
