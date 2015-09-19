<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
                 xmlns:ProductList="ext:ProductList">

  <xsl:output method="xml" omit-xml-declaration="yes"/> <!-- para que no ponga lo de la version de xml -->

  <xsl:param name="Product" />
  <xsl:param name="Seller" />
  <xsl:param name="Category" />
  <xsl:param name="Price" />
  <xsl:param name="Minutes2End" />
  <xsl:param name="ProductId" />

  <xsl:template match="/">
    <table class="table table-bordered table-stripped">
      <xsl:apply-templates/>
    </table>
  </xsl:template>

  <xsl:template match="product">
    <tr>
      <th><xsl:value-of select="$Product"/></th>
      <td>
        <xsl:element name ="a">
          <xsl:attribute name="href">
            ../Product/AddFavourite.aspx?id=<xsl:value-of select="$ProductId"/>
          </xsl:attribute>
          <span class="icon-star" ></span>
        </xsl:element>
        &#160;
        <xsl:element name ="a">
          <xsl:attribute name="href">
            ../Comment/AddComment.aspx?id=<xsl:value-of select="$ProductId"/>
          </xsl:attribute>
          <span class="icon-comment" > </span>
        </xsl:element>
        &#160;
        <xsl:value-of select="@name"/>
        <xsl:if test="ProductList:getNumberOfComment($ProductId) &gt; 0">
          &#160;
          <xsl:element name ="a">
            <xsl:attribute name="href">
              ../Comment/SeeComment.aspx?id=<xsl:value-of select="$ProductId"/>
            </xsl:attribute>
            <span class="icon-list-alt" > </span>
          </xsl:element>
        </xsl:if>
      </td>
    </tr>
    <tr>
      <th><xsl:value-of select="$Seller"/></th>
      <td>
        <xsl:element name ="a">
          <xsl:attribute name="href">
            ../Product/AddValuation.aspx?id=<xsl:value-of select="$ProductId"/>
          </xsl:attribute>
          <span class="icon-check" > </span>
        </xsl:element>
        &#160;
        <xsl:element name="a">
          <xsl:attribute name="href">
            ../Valuation/SeeValuation.aspx?seller=<xsl:value-of select="@owner"/>
          </xsl:attribute>
          <xsl:value-of select="@owner"/>
        </xsl:element>
      </td>
    </tr>
    <tr>
      <th><xsl:value-of select="$Category"/></th>
      <td><xsl:value-of select="@category"/></td>
    </tr>
    <tr>
      <th><xsl:value-of select="$Price"/></th>
      <td><xsl:value-of select="@cPrice"/></td>
    </tr>
    <tr>
      <th><xsl:value-of select="$Minutes2End"/></th>
      <td><xsl:value-of select="@minutesToEnd"/></td>
    </tr>
    </xsl:template>
  
</xsl:stylesheet>
