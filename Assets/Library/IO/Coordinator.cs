            Collider[] colls = Physics.OverlapSphere(
                v,
                (offset / 4),
                ~0,
                QueryTriggerInteraction.Collide);

            if (colls.Length == 1)
            {
                output.Add(colls[0].gameObject);
            }
            else if (colls.Length > 1)
            {
                bool add = true;

                foreach(Collider c in colls)
                {
                    //if any of the colliders are not the environment then don't add
                    if (c.gameObject.layer == LayerMask.NameToLayer("environment"))
                    {
                        continue;
                    }
                    else if (c.CompareTag("movement_tile"))
                    {
                        output.Add(c.gameObject);
                        continue;
                    }

                    add = false;
                }
            }
            else
            {
                //DO NOTHING
            }
        }

        Debug.Log("Found " + output.Count +" suitable tiles near " + tile.name);

        //Converting to array for output
        GameObject[] outArr = new GameObject[output.Count];
        output.CopyTo(outArr);
        return outArr;
    }
}
